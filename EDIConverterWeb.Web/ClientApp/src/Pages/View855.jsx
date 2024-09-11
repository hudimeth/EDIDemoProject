import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {Container, Button } from 'react-bootstrap'
import axios from 'axios';

const View855 = () => {

    const { referenceNumber } = useParams();

    const [ediText, setEdiText] = useState('');

    const load855 = async () => {
        const { data } = await axios.get(`/api/ediconverter/view855/?referenceNumber=${referenceNumber}`);
        setEdiText(data.ediText);
    }

    useEffect(() => {
        console.log(referenceNumber);
        load855();
    }, [])

    return (
        <Container>
            <pre>{ediText}</pre>
        </Container>
        )
    
}

export default View855;