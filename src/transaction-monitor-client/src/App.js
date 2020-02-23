import React from 'react';
import { Route, Switch, BrowserRouter as Router } from "react-router-dom";
import { Layout } from './components/Layout';
import { Home } from './pages/Home';
import { NotFound } from './pages/NotFound';

function App() {
    return (
        <>
            <Router>
                <Layout>
                    <Switch>
                        <Route exact path="/" component={Home} />
                        <Route component={NotFound} />
                    </Switch>
                </Layout>
            </Router>
        </>
    );
}

export default App;