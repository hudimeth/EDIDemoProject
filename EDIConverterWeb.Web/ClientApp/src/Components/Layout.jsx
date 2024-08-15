import { Container, Nav, Navbar } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from './AuthContextComponent';

const Layout = ({ children }) => {

    const { user } = useAuth();

    return (
        <Container fluid>
            <Navbar data-bs-theme='dark' expand='lg' className='bg-body-tertiary'>
                <Container>
                    <Navbar.Brand as={Link} to='/'>EDI Writer</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            {!user && <Nav.Link as={Link } to='/signup'>Signup</Nav.Link>}
                            {!user && <Nav.Link as={Link} to='/login'>Login</Nav.Link>}
                            {!!user && <Nav.Link as={Link} to='/create855'>855</Nav.Link>}
                            {!!user && <Nav.Link as={Link} to='/logout'>Logout</Nav.Link>}
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