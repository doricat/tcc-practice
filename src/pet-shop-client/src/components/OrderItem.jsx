import React from 'react';

const OrderItem = ({ info }) => {
    return (
        <>
            <div style={{ border: "1px solid #d6d8db", borderRadius: "0.25rem" }} className="mb-3">
                <div className="row">
                    <div className="col">
                        <ul className="list-inline">
                            <li className="list-inline-item">订单号：{info.id}</li>
                            <li className="list-inline-item">事务Id：{info.transactionId}</li>
                            <li className="list-inline-item">创建时间：{info.createdAt}</li>
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
                <div className="row">
                    <div className="col-md-3">
                        <img className="bd-placeholder-img"
                            width="100%"
                            height="100%"
                            alt="img"
                            src={info.items[0].image} />
                    </div>
                    <div className="col-md-3">{info.items[0].name}</div>
                    <div className="col-md-2">￥{info.items[0].price}</div>
                    <div className="col-md-2">{info.items[0].qty}</div>
                    <div className="col-md-2">{info.state}</div>
                </div>

                {/* <div className="btn-toolbar" role="toolbar">
                    <div className="btn-group mr-2" role="group">
                        <button type="button" className="btn btn-primary">确认事务</button>
                    </div>
                </div> */}
            </div>
        </>
    );
};

export { OrderItem };