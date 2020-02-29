import React from 'react';
import { Link } from 'react-router-dom';

const PetCard = () => {
    return (
        <>
            <div className="card">
                <img className="bd-placeholder-img card-img-top"
                    width="100%"
                    height="100%"
                    alt="img"
                    src="https://gss2.bdstatic.com/9fo3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike80%2C5%2C5%2C80%2C26/sign=f214803ab23eb13550cabfe9c777c3b6/267f9e2f070828386d2e70c6b699a9014d08f1c5.jpg" />

                <div className="card-body">
                    <h5 className="card-title">狸花猫</h5>
                    <p className="card-text">狸花猫是一种体格健壮的大型猫咪，长有美的斑纹被毛。尽管它感情不太外露，但还是能成为忠实友好的宠物。狸花猫以聪明的捕猎技巧而著称，需要较大的运动空间，所以不适宜小公寓的圈养生活。</p>
                    <Link to={"/item"} className="btn btn-primary">详细</Link>
                </div>
            </div>
        </>
    );
};

export { PetCard };