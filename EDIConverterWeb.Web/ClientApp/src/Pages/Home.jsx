﻿import React from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from '../Components/AuthContextComponent';

const Home = () => {
    const { user } = useAuth();

    return (
        <Container className='mt-5'>
            <h3 className='text-center mb-2'>Welcome to the EDI Writer</h3>
            <h5 className='text-center mb-5'>Use the links below to navigate</h5>
            <Row className='justify-content-md-center'>
                <Col>
                    <Button as={Link} to='create855' variant='primary' className='w-100'>855-Purchase Order Acknowledgement</Button>
                </Col>
                <Col>
                    <Button variant='primary' className='disabled w-100'>810-Invoice</Button>
                </Col>
            </Row>
        </Container >
    )
}

export default Home;