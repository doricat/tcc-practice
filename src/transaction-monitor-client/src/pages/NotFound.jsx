import React from 'react';
import Helmet from 'react-helmet';

const NotFound = () => {
    return (
        <>
            <Helmet title={"404 - Monitor"} />
            <div className="alert alert-info" role="alert">
                你迷路了
            </div>
        </>
    );
};

export { NotFound };