import React from "react";
import { Navbar as Nb, Nav } from "react-bootstrap";
import { Link } from "react-router-dom";

import {} from "react-router-dom";

const Navbar = () => {
  return (
    <Nb className="border-bottom shadow">
      <div className="container-fluid">
        <Nb.Brand href="/">Tips & Trick</Nb.Brand>
        <Nb.Toggle aria-controls="responsive-nav-nav" />
        <Nb.Collapse
          id="responsive-navbar-nav"
          className="d-sm-inline-flex justify-content-between"
        >
          <Nav className="mr-auto flex-grow-1">
            <Nav.Item>
              <Link to="/" className="nav-link text-dark">
                Trang chủ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to="/blog/about" className="nav-link text-dark">
                Giới thiệu
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to="/blog/contact" className="nav-link text-dark">
                Liên hệ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to="/blog/rss" className="nav-link text-dark">
                Rss Feed
              </Link>
            </Nav.Item>
          </Nav>
        </Nb.Collapse>
      </div>
    </Nb>
  );
};

export default Navbar;  // permit use Navbar in file other javascript