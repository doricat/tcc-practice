import React, { useState, useEffect } from 'react';
import { PetCard } from '../components/PetCard';
import { CardColumns } from 'react-bootstrap';
import Helmet from 'react-helmet';
import authService from '../services/AuthorizeService';

async function populatePetData(setState) {
    const token = await authService.getAccessToken();
    const response = await fetch('/products', {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const data = await response.json();
        setState(data.value);
    } else {
        console.error(response.status);
    }
}

const Home = () => {
    const [state, setState] = useState(null);
    useEffect(() => {
        populatePetData(setState);
    }, []);

    const loading = state == null ? <p>loading...</p> : null;
    const items = state != null ? state.map(x => {
        return <PetCard info={x} key={x.id} />
    }) : [];

    return (
        <>
            <Helmet title={"首页 - Pet shop"} />
            <CardColumns style={{ columnCount: "4" }}>
                {loading}
                {items}
            </CardColumns>
        </>
    );
}

export { Home };