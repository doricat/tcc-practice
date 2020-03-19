import React, { useState, useEffect } from 'react';
import Helmet from 'react-helmet';
import { Alert } from '../components/TransactionAlert';
import * as signalR from '@microsoft/signalr';

const Home = () => {
    const [state, setState] = useState({ tip: "Connecting...", items: [] });

    useEffect(() => {
        let connection = new signalR.HubConnectionBuilder()
            .withUrl("/api/transaction_hub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("ReceiveMessage", args => {
            var index = state.items.findIndex(x => x.sid === args.sid);
            if (index !== -1) {
                state.items[index].state = args.state;
                const nextState = { tip: undefined, items: state.items };
                setState(nextState);
            }
            else {
                state.items.unshift(args);
                const nextState = { tip: undefined, items: state.items };
                setState(nextState);
            }
        });

        connection.start().then(() => {
            const nextState = { tip: undefined, items: state.items };
            setState(nextState);
        });

        return () => connection.stop();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const tip = state.tip !== undefined
        ? (<div className="alert alert-primary" role="alert">
            {state.tip}
        </div>)
        : null;

    const items = state.items.map(x => (
        <Alert key={x.sid} state={x.state} serviceName={x.serviceName} tid={x.sid} beginTime={x.beginTime} expires={x.expires} />
    ));

    return (
        <>
            <Helmet title={"Home - Monitor"} />
            {tip}
            {items}
        </>
    );
};

export { Home };