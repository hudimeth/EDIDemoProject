import { Container, Button, Col, Form, Row, FloatingLabel, Alert } from 'react-bootstrap';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const Add850 = () => {

    const navigate = useNavigate();

    const [purchaseOrder, setPurchaseOrder] = useState('');
    const [isValidForm, setIsValidForm] = useState(true);

    const isValidData = !!purchaseOrder

    const onPurchaseOrderTextChange = e => {
        setPurchaseOrder(e.target.value);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        if (isValidData) {
            const { data } = await axios.post('/api/ediconverter/add850', { purchaseOrder: purchaseOrder });
            if (!data.id) {
                setIsValidForm(false);
            } else {
            navigate(`/view855/${data.id}`);
            }
        }
    }

    return (
        <Container className='pt-3'>
            {!isValidForm && <Alert variant='danger'>
                Invalid information entered! Please correct and resubmit.
            </Alert>}
            <h1 className='text-center mb-2'>850</h1>
            <h4 className='text-center'>Paste the 850 Text Below</h4>
            <Form onSubmit={onFormSubmit}>
                <FloatingLabel className='mb-3' label='850 Text'>
                    <Form.Control
                        as='textarea'
                        placeholder='850 Text'
                        style={{ height: '100px' }}
                        name='purchaseOrder'
                        value={purchaseOrder}
                        onChange={onPurchaseOrderTextChange} />
                </FloatingLabel>
                <Row>
                    <Col></Col>
                    <Col>
                        <Button variant="primary" className='w-100' type="submit" disabled={!isValidData}>Create EDI Text</Button>
                    </Col>
                    <Col></Col>
                </Row>
            </Form>
        </Container>
    );
}

export default Add850;



