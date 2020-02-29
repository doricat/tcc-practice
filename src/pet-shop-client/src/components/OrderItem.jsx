import React from 'react';

const OrderItem = () => {
    return (
        <>
            <div style={{ border: "1px solid #d6d8db", borderRadius: "0.25rem" }} className="mb-3">
                <div className="row">
                    <div className="col">
                        <ul className="list-inline">
                            <li className="list-inline-item">订单号：123456789</li>
                            <li className="list-inline-item">事务Id：123456789</li>
                        </ul>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-3"><p>图片</p></div>
                    <div className="col-md-3"><p>名称</p></div>
                    <div className="col-md-2"><p>单价</p></div>
                    <div className="col-md-2"><p>数量</p></div>
                    <div className="col-md-2"><p>事务状态</p></div>
                </div>
                <div className="row mb-3">
                    <div className="col-md-3">
                        <img className="bd-placeholder-img"
                            width="100%"
                            height="100%"
                            alt="img"
                            src="https://gss2.bdstatic.com/9fo3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike80%2C5%2C5%2C80%2C26/sign=f214803ab23eb13550cabfe9c777c3b6/267f9e2f070828386d2e70c6b699a9014d08f1c5.jpg" />
                    </div>
                    <div className="col-md-3">狸花猫</div>
                    <div className="col-md-2">￥100</div>
                    <div className="col-md-2">1</div>
                    <div className="col-md-2">Pending</div>
                </div>

                <div className="btn-toolbar" role="toolbar">
                    <div className="btn-group mr-2" role="group">
                        <button type="button" className="btn btn-primary">确认事务</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export { OrderItem };