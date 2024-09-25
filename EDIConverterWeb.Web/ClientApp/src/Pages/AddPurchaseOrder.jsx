import { Container, Button, Col, Form, Row, FloatingLabel, Alert } from 'react-bootstrap';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const AddPurchaseOrder = () => {

    const navigate = useNavigate();

    const [purchaseOrder, setPurchaseOrder] = useState('');
    const [isValidForm, setIsValidForm] = useState(true);

    const isValidData = !!purchaseOrder

    const onPurchaseOrderTextChange = e => {
        setPurchaseOrder(e.target.value);
    }

    const getCurrentDate = () => {
        const date = new Date();
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();

        if (day < 10) {
            day = `0${day}`;
        }
        if (month < 10) {
            month = `0${month}`;
        }
        return `${year}${month}${day}`;
    }

    const downloadPurchaseOrderFile = async (data) => {
        const { data: file } = await axios.get(`/api/ediconverter/getpurchaseorderfile?purchaseorderid=${data.purchaseOrderId}`, { responseType: 'blob' })
        const href = URL.createObjectURL(file);
        const link = document.createElement('a');
        link.href = href;
        link.setAttribute('download', `CRT-TRIS_${getCurrentDate()}_${data.purchaseOrderNumber}_850.txt`);
        document.body.appendChild(link);
        link.click();

        document.body.removeChild(link);
        URL.revokeObjectURL(href);
    } 

    const downloadPOAcknowledgementFile = async (data) => {
        const { data: file } = await axios.get(`/api/ediconverter/get855file?referencenumber=${data.poAcknowledgementReferenceNumber}`, { responseType: 'blob' })
        const href = URL.createObjectURL(file);
        const link = document.createElement('a');
        link.href = href;
        link.setAttribute('download', `CRT-TRIS_${getCurrentDate()}_${data.purchaseOrderNumber}_855.edi`);
        document.body.appendChild(link);
        link.click();

        document.body.removeChild(link);
        URL.revokeObjectURL(href);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        if (isValidData) {
            const { data } = await axios.post('/api/ediconverter/addpurchaseorder', { purchaseOrder: purchaseOrder });
            if (!data.purchaseOrderId || !data.purchaseOrderNumber || !data.poAcknowledgementReferenceNumber) {
                setIsValidForm(false);
            } else {
                await downloadPurchaseOrderFile(data);
                await downloadPOAcknowledgementFile(data);
                navigate('/');
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
            {/*{!!purchaseOrderText && <pre>{purchaseOrderText }</pre> }*/}
        </Container>
    );
}

export default AddPurchaseOrder;



