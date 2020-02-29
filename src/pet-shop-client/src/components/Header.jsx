import React from 'react';
import { Link } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { LoginMenu } from './LoginMenu';
import './header.css';

const Header = () => {
    const [state, setState] = React.useState({ collapsed: true });

    const toggleNavbar = () => {
        const nextState = {
            collapsed: state.collapsed
        };
        setState(nextState);
    }

    return (
        <>
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">PetShop</NavbarBrand>
                        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/shopping_car">购物车</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/order">订单</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/bill">账单</NavLink>
                                </NavItem>
                                <LoginMenu />
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        </>
    );
};

export { Header };