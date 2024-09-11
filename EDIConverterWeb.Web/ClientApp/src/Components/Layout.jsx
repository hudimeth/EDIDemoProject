import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from './Authentication/AuthContextComponent';

const Layout = ({ children }) => {

    const { user } = useAuth();

    return (
        <Container fluid>
            <Navbar data-bs-theme='dark' expand='lg' className='bg-body-tertiary'>
                <Container>
                    <Navbar.Brand as={Link} to='/'>EDI Converter</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            {!user && <Nav.Link as={Link } to='/adduser'>Add User</Nav.Link>}
                            {!user && <Nav.Link as={Link} to='/login'>Login</Nav.Link>}
                            {!!user && <Nav.Link as={Link} to='/add850'>Enter 850</Nav.Link>}
                            {!!user && <NavDropdown title={`${user.firstName} ${user.lastName}` }>
                                <NavDropdown.Item as={Link } to='/logout'>Logout</NavDropdown.Item>
                            </NavDropdown>}
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container className="container" style={{ marginTop: 50 }}>
                {children}
            </Container>
        </Container>
    );
}

export default Layout;