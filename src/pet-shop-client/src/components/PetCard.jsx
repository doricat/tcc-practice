import React from 'react';
import { Link } from 'react-router-dom';

const PetCard = ({ info }) => {
    return (
        <>
            <div className="card">
                <img className="bd-placeholder-img card-img-top"
                    width="100%"
                    height="100%"
                    alt="img"
                    src={info.image} />

                <div className="card-body">
                    <h5 className="card-title">{info.name}</h5>
                    <p className="card-text">{info.description}</p>
                    <Link to={`/item/${info.id}`} className="btn btn-primary">详细</Link>
                </div>
            </div>
        </>
    );
};

export { PetCard };