import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../Components/Authentication/AuthContextComponent';
import { Container, Button, Col, Form, Row, FloatingLabel } from 'react-bootstrap';

const Login = () => {

    const { setUser } = useAuth();
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [isValidLogin, setIsValidLogin] = useState(true);

    const onTextChange = e => {
        const copy = { ...formData };
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        const { data } = await axios.post('/api/account/login', formData);
        const isValid = !!data;
        setIsValidLogin(isValid);
        if (isValid) {
            setUser(data);
            navigate('/');
        }
    }

    return (
        <Container className='container pt-5'>
            <Row style={{ display: 'flex', alignItems: 'center' }}>
                <Container className='col-md-6 offset-md-3 bg light p-4 rounded shadow'>
                    <h3 className='text-center' >Login</h3>
                    {!isValidLogin && <span className='text-danger'>Invalid username/password. Please try again.</span>}
                    <Form onSubmit={onFormSubmit}>
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
                                <Button variant="primary" className='w-100' type="submit">Login</Button>
                            </Col>
                            <Col></Col>
                        </Row>
                    </Form>
                </Container>
            </Row>
        </Container>
    )
}
export default Login;