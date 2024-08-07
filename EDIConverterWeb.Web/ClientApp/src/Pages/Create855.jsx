import { Container, Button, Col, Form, Row, FloatingLabel, Alert } from 'react-bootstrap';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const Create855 = () => {

    //make the delete buttons work
    //keep track of the items

    const navigate = useNavigate();

    const [purchaseOrderAcknowledgement, setPurchaseOrderAcknowledgement] = useState({
        purchaseOrderNumber: '',
        itemsList: ''
    });
    const [poDate, setPoDate] = useState('');
    const [testIndicator, setTestIndicator] = useState('P');
    const [isValidForm, setIsValidForm] = useState(true);

    const isValidData = poDate.length === 8 && purchaseOrderAcknowledgement.purchaseOrderNumber.length && purchaseOrderAcknowledgement.itemsList && testIndicator

    //const [items, setItems] = useState([]);
    //const [indexCounter, setIndexCounter] = useState(0);

    const onTextChange = e => {
        const copy = { ...purchaseOrderAcknowledgement };
        copy[e.target.name] = e.target.value;
        setPurchaseOrderAcknowledgement(copy);
        //console.log(purchaseOrderAcknowledgement);
    }

    const onPoDateChange = e => {
        if (!isNaN(e.target.value)) {
            setPoDate(e.target.value)
        }
    }

    const onTestIndicatorChange = e => {
        if (testIndicator) {
            setTestIndicator(e.target.value);
        }
        if (e.target.value === 'T' || e.target.value === 'P') {
            setTestIndicator(e.target.value);
        }
    }

    //const incrementTemporaryId = () => {
    //    let copy = indexCounter;
    //    copy += 1;
    //    //console.log(copy );
    //    setIndexCounter(copy);
    //}

    //const onAddItemRowClicked = () => {
    //    //the array isn't stored in items[]
    //    //need to debug
    //    console.log(items);
    //    const copy = [...items];
    //    copy.push({
    //        temporaryId: indexCounter,
    //        quantity: 0,
    //        UnitPrice: 0,
    //        ItemNumber: ''
    //    });
    //    setItems(copy);
    //    incrementTemporaryId();
    //    console.log(copy);
    //}

    //const onDeleteItemClicked = id => {
    //    const copy = [...items];
    //    setItems(copy.filter(i => i.temporaryId == !id));
    //}

    const onFormSubmit = async e => {
        e.preventDefault();
        if (isValidData) {
            const { data } = await axios.post('/api/ediconverter/create855', {
                purchaseOrderNumber: purchaseOrderAcknowledgement.purchaseOrderNumber,
                purchaseOrderDate: poDate,
                testIndicator: testIndicator,
                itemsList: purchaseOrderAcknowledgement.itemsList
            });
            console.log(data.id);
            if (data.id === 0) {
                setIsValidForm(false);
            } else {
                navigate(`/view855/${data.id}`);
            }
        }
    }

    //input to write how many items were ordered:
    //it was tied to the items.length in state
    //it worked to add items, but when i tried to delete there was a bug-
    //it automatically puts in "1"
    //const onAmountItemsChanged = e => {
    //    if (e.target.value > items.length) {
    //        const copy = [...items];
    //        const copy2 = Array(e.target.value - items.length);
    //        const finalList = [...copy, ...copy2];
    //        console.log(`copy: ${copy}, copy2:${copy2}, finalList:${finalList}`);
    //        setItems(finalList);
    //    }
    //    else {
    //        console.log(`length: ${items.length}`);
    //        setItems(Array(e.target.value));
    //        console.log(e.target.value);
    //    }
    //}

    ////part of the return statement:
    //<Form.Group as={Row} sm={6} className='mb-3'>
    //    <Form.Label as={Col}>Amount of items ordered:</Form.Label>
    //    <Col>
    //        <Form.Control type='number' value={items.length} onChange={onAmountItemsChanged} />
    //    </Col>
    //</Form.Group>

    ////creating items based on inputs:
    //    < Row >
    //    <Table striped bordered hover className='mb-5'>
    //        <thead>
    //            <tr>
    //                <th></th>
    //                <th>Quantity Ordered</th>
    //                <th>Unit of Measure</th>
    //                <th>Unit Price</th>
    //                <th>Item Number</th>
    //                <th><Button variant='secondary' className='w-100' onClick={onAddItemRowClicked}>Add Item Row</Button></th>
    //            </tr>
    //        </thead>
    //        <tbody>
    //            {items.map(i =>
    //                <tr key={i.temporaryId}>
    //                    <td>{i.temporaryId}</td>
    //                    <td><Form.Control type='number' /></td>
    //                    <td>
    //                        <Form.Select aria-label="Default select example">
    //                            <option>Choose...</option>
    //                            <option value="1">One</option>
    //                            <option value="2">Two</option>
    //                            <option value="3">Three</option>
    //                        </Form.Select>
    //                    </td>
    //                    <td><Form.Control type='number' placeholder='an amount with 2 decimal places' /></td>
    //                    <td><Form.Control type='text' /></td>
    //                    <td><Button variant='danger' className='w-100' onClick={onDeleteItemClicked(i.temporaryId)}>Delete Item</Button></td>
    //                </tr >)}
    //        </tbody>
    //    </Table>
    //            </Row >

    return (
        <Container className='pt-5'>
            {!isValidForm && <Alert variant='danger'>
                Invalid information entered! Please correct and resubmit.
            </Alert>}
            <h1 className='text-center mb-2'>855</h1>
            <h4 className='text-center'>Fill in the appropriate information:</h4>
            <Form onSubmit={onFormSubmit}>
                <Row className='mt-4'>
                    <FloatingLabel className='gx-1 mx-2'
                        as={Col}
                        md={5}
                        label='PO Number (up to 22 characters)'>
                        <Form.Control type='text'
                            placeholder='PO Number'
                            maxLength='22'
                            name='purchaseOrderNumber'
                            value={purchaseOrderAcknowledgement.purchaseOrderNumber}
                            onChange={onTextChange} />
                    </FloatingLabel>
                    <FloatingLabel className='gx-1 mx-2'
                        as={Col}
                        md={4}
                        label='PO Date (YYYYMMDD)'>
                        <Form.Control type='text'
                            placeholder='PO Date'
                            maxLength='8'
                            value={poDate}
                            onChange={onPoDateChange} />
                    </FloatingLabel>
                    <FloatingLabel className='gx-1 mx-2'
                        as={Col}
                        md={2}
                        label='Test Indicator (P or T)'>
                        <Form.Control type='text'
                            placeholder='Test Indicator'
                            maxLength='1'
                            value={testIndicator}
                            onChange={onTestIndicatorChange} />
                    </FloatingLabel>
                </Row>
                <h6 className='text-center mt-4'>Ordered Items:</h6>
                <FloatingLabel className='mb-3' label='Paste PO1 lines here. Make sure to put in the full lines, including the "~"'>
                    <Form.Control
                        as='textarea'
                        placeholder='Paste PO1 lines here'
                        style={{ height: '100px' }}
                        name='itemsList'
                        value={purchaseOrderAcknowledgement.itemsList}
                        onChange={onTextChange} />
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

export default Create855;



