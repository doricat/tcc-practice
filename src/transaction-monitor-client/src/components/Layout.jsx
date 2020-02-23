import React from 'react';
import { Header } from './Header';

const Layout = (props) => {
    return (
        <>
            <Header />
            <div className="container">
                {props.children}
            </div>
        </>
    );
};

export { Layout };