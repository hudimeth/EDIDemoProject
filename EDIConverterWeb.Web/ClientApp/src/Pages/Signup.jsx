import React, { useState } from 'react';
import { Container, Button, Col, Form, Row, FloatingLabel} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'
import axios from 'axios';

const Signup = () => {
    //didn't put extra validation logic here
    //because we'll take this out

    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: ''
    })

    const onTextChange = e => {
        const copy = { ...formData }
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        await axios.post('/api/account/signup', formData);
        navigate('/login');
    }

    return (
        <Container className='container pt-5'>
            <Row style={{ display: 'flex', alignItems: 'center' }}>
                <Container className='col-md-6 offset-md-3 bg light p-4 rounded shadow'>
                    <h3 className='text-center' >Signup for a new account</h3>
                    <Form onSubmit={onFormSubmit}>
                        <FloatingLabel className='gx-1 my-2'
                            label='First Name'>
                            <Form.Control type='text'
                                placeholder='First Name'
                                name='firstName'
                                value={formData.firstName}
                                onChange={onTextChange} />
                        </FloatingLabel>
                        <FloatingLabel className='gx-1 my-2'
                            label='Last Name'>
                            <Form.Control type='text'
                                placeholder='Last Name'
                                name='lastName'
                                value={formData.lastName}
                                onChange={onTextChange} />
                        </FloatingLabel>
                        <FloatingLabel className='gx-1 my-2'
                            label='Email'>
                            <Form.Control type='email'
                                placeholder='Email'
                                name='email'
                                value={formData.email}
                                onChange={onTextChange} />
                        </FloatingLabel>
                        <FloatingLabel className='gx-1 my-2'
                            label='Password'>
                            <Form.Control type='password'
                                placeholder='Password'
                                name='password'
                                value={formData.password}
                                onChange={onTextChange} />
                        </FloatingLabel>
                        <Row>
                            <Col></Col>
                            <Col>
                                <Button variant="primary" className='w-100' type="submit">Signup</Button>
                            </Col>
                            <Col></Col>
                        </Row>
                    </Form>
                </Container>
            </Row>
        </Container>
    )
}
export default Signup;