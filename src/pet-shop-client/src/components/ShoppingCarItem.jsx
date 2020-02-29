import React from 'react';

const ShoppingCarItem = () => {
    return (
        <>
            <div className="card mb-3" >
                <div className="row no-gutters">
                    <div className="col-md-4">
                        <img className="bd-placeholder-img"
                            width="100%"
                            height="100%"
                            alt="img"
                            src="https://gss2.bdstatic.com/9fo3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike80%2C5%2C5%2C80%2C26/sign=f214803ab23eb13550cabfe9c777c3b6/267f9e2f070828386d2e70c6b699a9014d08f1c5.jpg" />
                    </div>
                    <div className="col-md-8">
                        <div className="card-body">
                            <h5 className="card-title">狸花猫</h5>
                            <div className="row">
                                <div className="col">
                                    <div className="btn-toolbar mb-3" role="toolbar" aria-label="Toolbar with button groups">
                                        <div className="input-group mr-2">
                                            <div className="input-group-append">
                                                <div className="input-group-text" style={{
                                                    border: "none", borderRadius: 0, color: "red", fontWeight: "bold"
                                                }}>￥100</div>
                                            </div>
                                        </div>

                                        <div className="input-group mr-2">
                                            <div className="input-group-prepend">
                                                <button className="btn btn-outline-secondary" type="button">-</button>
                                            </div>
                                            <input type="text" className="form-control" defaultValue="1" style={{ "width": "50px" }} />
                                            <div className="input-group-append">
                                                <button className="btn btn-outline-secondary" type="button">+</button>
                                            </div>
                                        </div>

                                        <div className="btn-group">
                                            <button type="button" className="btn btn-danger">删除</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export { ShoppingCarItem };