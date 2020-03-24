import React, { useState, useEffect } from 'react';
import Helmet from 'react-helmet';
import { OrderItem } from '../components/OrderItem';
import authService from '../services/AuthorizeService';
import { push } from 'connected-react-router';
import { useDispatch } from 'react-redux';
import { ApplicationPaths } from '../services/ApiAuthorizationConstants';

async function populateOrderData(setState, dispatch) {
    const token = await authService.getAccessToken();
    const response = await fetch('/orders', {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const data = await response.json();
        setState(data.value);
    } else if (response.status === 401) {
        dispatch(push(ApplicationPaths.Login));
    } else {
        console.error(response.status);
    }
}

const Order = () => {
    const [state, setState] = useState(null);
    const dispatch = useDispatch();

    useEffect(() => {
        populateOrderData(setState, dispatch);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loading = state == null ? <p>loading...</p> : null;
    const items = state != null ? state.map(x => {
        return <OrderItem info={x} key={x.id} />
    }) : [];
    const alert = state != null && items.length === 0 ? <div className="alert alert-info" role="alert">无数据</div> : null;

    return (
        <>
            <Helmet title={"订单 - Pet shop"} />
            {loading}
            {items}
            {alert}
        </>
    );
};

export { Order };