import React, { useState, useEffect } from 'react';
import Helmet from 'react-helmet';
import authService from '../services/AuthorizeService';
import { useParams } from 'react-router-dom';
import { push } from 'connected-react-router';
import { useDispatch } from 'react-redux';
import { ApplicationPaths } from '../services/ApiAuthorizationConstants';

async function populatePetData(id, setState, dispatch) {
    const token = await authService.getAccessToken();
    const response = await fetch(`/products/${id}`, {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const data = await response.json();
        setState(data.value);
    } else if (response.status === 404) {
        dispatch(push("/404"));
    } else {
        console.error(response.status);
    }
}

async function buy(id, dispatch) {
    const token = await authService.getAccessToken();
    let headers = !token ? {} : { "Authorization": `Bearer ${token}` };
    headers["Content-Type"] = "application/json";
    const response = await fetch('/orders', {
        method: "POST",
        headers,
        body: JSON.stringify({ productId: id })
    });

    if (response.ok) {
        dispatch(push("/order"));
    } else if (response.status === 401) {
        dispatch(push(ApplicationPaths.Login));
    } else {
        console.error(response.status);
    }
}

const PetItem = () => {
    const [state, setState] = useState(null);
    let { id } = useParams();
    const dispatch = useDispatch();

    useEffect(() => {
        populatePetData(id, setState, dispatch);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loading = state == null ? <p>loading...</p> : null;
    const item = state != null ? (
        <div className="row">
            <div className="col-5">
                <img className="bd-placeholder-img"
                    width="100%"
                    height="100%"
                    alt="img"
                    src={state.image} />
            </div>
            <div className="col-7">
                <div className="row">
                    <div className="col">
                        <h4>{state.name}</h4>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <p>{state.description}</p>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <div className="btn-toolbar mb-3" role="toolbar" aria-label="Toolbar with button groups">
                            <div className="input-group mr-2">
                                <div className="input-group-append">
                                    <div className="input-group-text" style={{
                                        border: "none", borderRadius: 0, color: "red", fontWeight: "bold"
                                    }}>￥{state.price}</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <div className="btn-group" role="group">
                            <button type="button" className="btn btn-primary" onClick={() => buy(state.id, dispatch)}>购买</button>
                            {/* <button type="button" class="btn btn-secondary">延迟提交事务购买</button> */}
                        </div>
                    </div>
                </div>

            </div>
        </div>
    ) : null;
    const title = state != null ? state.name : "loading";

    return (
        <>
            <Helmet title={`${title} - Pet shop`} />
            {loading}
            {item}
        </>
    );
};

export { PetItem };