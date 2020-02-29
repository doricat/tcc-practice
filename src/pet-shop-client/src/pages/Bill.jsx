import React from 'react';
import Helmet from 'react-helmet';
import { BillItem } from '../components/BillItem';

const Bill = () => {
    return (
        <>
            <Helmet title={"账单 - Pet shop"} />
            <BillItem />
            <BillItem />
            <BillItem />
            <BillItem />
        </>
    );
};

export { Bill };