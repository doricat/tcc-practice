import React from 'react';

const Alert = ({ state, serviceName, tid, beginTime, expires }) => {
    let header;
    let css = ["alert"];
    switch (state.toLowerCase()) {
        case "pending":
            header = "Pending";
            css.push("alert-primary");
            break;
        case "confirmed":
            header = "Confirmed";
            css.push("alert-success");
            break;
        case "canceled":
            header = "Canceled";
            css.push("alert-warning");
            break;
        default:
            throw new Error(`ArgumentOutOfRangeException: ${state}`);
    }

    return (
        <div className={css.join(" ")} role="alert">
            {/* <button type="button" className="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button> */}
            <h4 className="alert-heading">
                {header}
            </h4>

            <dl className="row mb-0">
                <dt className="col-sm-3">Service Name</dt>
                <dd className="col-sm-9">{serviceName}</dd>

                <dt className="col-sm-3">Transaction Id</dt>
                <dd className="col-sm-9">{tid}</dd>

                <dt className="col-sm-3">BeginTime</dt>
                <dd className="col-sm-9">{beginTime}</dd>

                <dt className="col-sm-3">Expires</dt>
                <dd className="col-sm-9">{expires}</dd>
            </dl>
        </div>
    );
};

export { Alert };