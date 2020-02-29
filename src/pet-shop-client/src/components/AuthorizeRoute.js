import React, { useState, useEffect } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { ApplicationPaths, QueryParameterNames } from '../services/ApiAuthorizationConstants';
import authService from '../services/AuthorizeService';

async function populateAuthenticationState(setState) {
    const authenticated = await authService.isAuthenticated();
    setState({ ready: true, authenticated });
}

async function authenticationChanged(setState) {
    setState({ ready: false, authenticated: false });
    await populateAuthenticationState();
}

const AuthorizeRoute = (props) => {
    const [state, setState] = useState({
        ready: false,
        authenticated: false
    });

    useEffect(() => {
        const subscription = authService.subscribe(() => authenticationChanged(setState));
        populateAuthenticationState(setState);
        return () => authService.unsubscribe(subscription);
    }, []);

    const { ready, authenticated } = state;
    const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI(window.location.href)}`
    if (!ready) {
        return <div></div>;
    } else {
        const { component: Component, ...rest } = props;
        return <Route {...rest}
            render={(props) => {
                if (authenticated) {
                    return <Component {...props} />
                } else {
                    return <Redirect to={redirectUrl} />
                }
            }} />;
    }
};

export { AuthorizeRoute };