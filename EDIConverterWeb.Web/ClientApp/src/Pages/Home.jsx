import React from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from '../Components/Authentication/AuthContextComponent';

const Home = () => {
    const { user } = useAuth();

    return (
        <Container className='mt-5'>
            <h3 className='text-center mb-2'>Welcome to the EDI Converter</h3>
            <h5 className='text-center mb-5'>Use the links below to navigate</h5>
            <Row className='justify-content-md-center'>
                <Col>
                    <Button as={Link} to='add850' variant='primary' className='w-100'>Enter 850- Purchase Order</Button>
                </Col>
                <Col>
                    <Button variant='primary' className='disabled w-100'>810-Invoice</Button>
                </Col>
            </Row>
        </Container >
    )
}

export default Home;