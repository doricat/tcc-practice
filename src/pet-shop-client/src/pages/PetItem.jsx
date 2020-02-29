import React from 'react';
import Helmet from 'react-helmet';

const PetItem = () => {
    return (
        <>
            <Helmet title={"狸花猫 - Pet shop"} />
            <div className="row">
                <div className="col-5">
                    <img className="bd-placeholder-img"
                        width="100%"
                        height="100%"
                        alt="img"
                        src="https://gss2.bdstatic.com/9fo3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike80%2C5%2C5%2C80%2C26/sign=f214803ab23eb13550cabfe9c777c3b6/267f9e2f070828386d2e70c6b699a9014d08f1c5.jpg" />
                </div>
                <div className="col-7">
                    <div className="row">
                        <div className="col">
                            <h4>狸花猫</h4>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <p>狸花猫是一种体格健壮的大型猫咪，长有美的斑纹被毛。尽管它感情不太外露，但还是能成为忠实友好的宠物。狸花猫以聪明的捕猎技巧而著称，需要较大的运动空间，所以不适宜小公寓的圈养生活。</p>
                        </div>
                    </div>

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
                                    <div className="input-group-append">
                                        <div className="input-group-text" style={{
                                            border: "none", borderRadius: 0
                                        }}>库存：1，不可用：1</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="btn-toolbar mb-3" role="toolbar" aria-label="Toolbar with button groups">
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
                                    <button type="button" className="btn btn-primary">加入购物车</button>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </>
    );
};

export { PetItem };