import React from 'react';
import Helmet from 'react-helmet';
import { OrderItem } from '../components/OrderItem';

const Order = () => {
    return (
        <>
            <Helmet title={"订单 - Pet shop"} />
            <OrderItem />
            <OrderItem />
        </>
    );
};

export { Order };