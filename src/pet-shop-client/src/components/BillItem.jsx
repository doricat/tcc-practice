import React from 'react';

const BillItem = ({ info }) => {
    return (
        <>
            <div style={{ border: "1px solid #d6d8db", borderRadius: "0.25rem" }} className="mb-3">
                <div className="row">
                    <div className="col-md-3"><p>支付Id</p></div>
                    <div className="col-md-3"><p>事务Id</p></div>
                    <div className="col-md-3"><p>金额</p></div>
                    <div className="col-md-3"><p>事务状态</p></div>
                </div>
                <div className="row">
                    <div className="col-md-3">{info.id}</div>
                    <div className="col-md-3">{info.transactionId}</div>
                    <div className="col-md-3">￥{info.amount}</div>
                    <div className="col-md-3">{info.state}</div>
                </div>
            </div>
        </>
    );
};

export { BillItem };