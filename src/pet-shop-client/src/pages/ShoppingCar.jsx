import React from 'react';
import { ShoppingCarItem } from '../components/ShoppingCarItem';
import Helmet from 'react-helmet';

const ShoppingCar = () => {
    return (
        <>
            <Helmet title={"购物车 - Pet shop"} />
            <ShoppingCarItem />

            <div className="btn-toolbar mb-3" role="toolbar">
                <div className="input-group-append">
                    <div className="input-group-text" style={{
                        border: "none", borderRadius: 0, color: "red", fontWeight: "bold"
                    }}>总计：￥100</div>
                </div>
            </div>

            <div className="btn-toolbar" role="toolbar">
                <div className="btn-group mr-2" role="group">
                    <button type="button" className="btn btn-primary">正常提交订单并支付，自动事务提交</button>
                </div>

                <div className="btn-group mr-2" role="group">
                    <button type="button" className="btn btn-primary">正常提交订单并支付，手动事务提交</button>
                </div>
            </div>
        </>
    );
};

export { ShoppingCar };