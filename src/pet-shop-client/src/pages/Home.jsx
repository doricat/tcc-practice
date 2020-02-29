import React from 'react';
import { PetCard } from '../components/PetCard';
import { CardColumns } from 'react-bootstrap';
import Helmet from 'react-helmet';

const Home = () => {
    return (
        <>
            <Helmet title={"首页 - Pet shop"} />
            <CardColumns style={{ columnCount: "4" }}>
                <PetCard />
            </CardColumns>
        </>
    );
}

export { Home };