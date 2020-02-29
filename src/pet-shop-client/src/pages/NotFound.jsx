import React from 'react';
import Helmet from 'react-helmet';

const NotFound = () => {
    return (
        <>
            <Helmet title={"404 - Pet Shop"} />
            <div className="alert alert-info" role="alert">
                你迷路了
            </div>
        </>
    );
};

export { NotFound };