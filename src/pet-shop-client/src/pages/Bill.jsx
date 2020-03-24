import React, { useState, useEffect } from 'react';
import Helmet from 'react-helmet';
import { BillItem } from '../components/BillItem';
import authService from '../services/AuthorizeService';
import { push } from 'connected-react-router';
import { useDispatch } from 'react-redux';
import { ApplicationPaths } from '../services/ApiAuthorizationConstants';

async function populateBillData(setState, dispatch) {
    const token = await authService.getAccessToken();
    const response = await fetch('/payment_accounts', {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const data = await response.json();
        setState(data.value.items);
    } else if (response.status === 404) {
        dispatch(push(ApplicationPaths.Login));
    } else {
        console.error(response.status);
    }
}

const Bill = () => {
    const [state, setState] = useState(null);
    const dispatch = useDispatch();

    useEffect(() => {
        populateBillData(setState, dispatch);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loading = state == null ? <p>loading...</p> : null;
    const items = state != null ? state.map(x => {
        return <BillItem info={x} key={x.id} />
    }) : [];
    const alert = state != null && items.length === 0 ? <div className="alert alert-info" role="alert">无数据</div> : null;

    return (
        <>
            <Helmet title={"账单 - Pet shop"} />
            {loading}
            {items}
            {alert}
        </>
    );
};

export { Bill };