﻿import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { Container, Row, Col, Card, CardHeader, CardBody } from 'reactstrap';
import { Form, FormGroup, Button, Input } from 'reactstrap';
import { Modal, ModalHeader, ModalBody, Progress } from 'reactstrap';
import BootstrapSwitchButton from 'bootstrap-switch-button-react';
import InputGroup from './util/PrefixInputGroup';
import classNames from 'classnames';

import map from 'lodash/map';
import transform from 'lodash/transform';
import chunk from 'lodash/chunk';
import range from 'lodash/range';
import attempt from 'lodash/attempt';
import defaultTo from 'lodash/defaultTo';

import { encode } from 'slugid';

const InputWithoutSpinner = styled(Input)`
  /* For firefox */
  appearance: textfield;
  /* For Chromium */
  &::-webkit-inner-spin-button,
  &::-webkit-outer-spin-button {
    appearance: none;
  }
`;

export default function Configure(props) {
    const randomizer_id = props.match.params.randomizer_id;

    const [options, setOptions] = useState(null);
    const [names, setNames] = useState({});
    const [randomizer, setRandomizer] = useState(null);
    const [modal, setModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);

    useEffect(() => {
        attempt(async () => {
            try {
                var response = await fetch(`/api/randomizers/${randomizer_id}`);
                if (response) {
                    const result = await response.json();
                    const options = transform(result.options,
                        (options, opt) => options[opt.key] = defaultTo(opt.default, ''),
                        {}
                    );
                    setOptions(options);
                    setRandomizer(result);
                } else {
                    setErrorMessage('Cannot load metadata for the specified randomizer.');
                }
            } catch (error) {
                setErrorMessage(error);
            }
        });
    }, []); /* eslint-disable-line react-hooks/exhaustive-deps */

    async function createGame() {
        setModal(true);

        try {
            if (options.gamemode === 'multiworld') {
                for (let p = 0; p < parseInt(options.players); p++) {
                    options[`player-${p}`] = names[p];
                }
            }

            const response = await fetch(`/api/randomizers/${randomizer_id}/generate`, {
                method: 'POST',
                cache: 'no-cache',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(options)
            });
            const data = await response.json();
            setModal(false);
            if (options.gamemode === 'multiworld') {
                props.history.push(`/multiworld/${encode(data.guid)}`);
            } else {
                props.history.push(`/seed/${encode(data.guid)}`);
            }
        } catch (error) {
            console.log(error);
            setModal(false);
        }
    }

    function updateOption(key, value) {
        setOptions({ ...options, [key]: value });
    }

    const component = randomizer
        ? (<>
            <CardHeader className="bg-primary text-white">
                {randomizer.name} - {randomizer.version}
            </CardHeader>
            <CardBody>
                <Form autoComplete="off" onSubmit={(e) => { e.preventDefault(); createGame(); }}>
                    {map(chunk(randomizer.options, 2), (group, i) => (
                        <FormGroup row={true} key={i}>
                            {map(group, (option) => {
                                let input = createFormOption(option);
                                return input && <Col key={option.key} md="6">{input}</Col>;
                            })}
                        </FormGroup>
                    ))}
                    {options.gamemode === 'multiworld' && (
                        <FormGroup row={true}>
                            {map(createPlayerInputs(), (input, p) => (
                                <Col key={`playerInput${p}`} md={{ size: 5, offset: 1 - (p % 2) }}>
                                    {input}
                                </Col>
                            ))}
                        </FormGroup>
                    )}
                    <FormGroup row={true}>
                        <Col md="6">
                            <Button color="success" type="submit">Generate Game</Button>
                        </Col>
                    </FormGroup>
                </Form>
            </CardBody>
        </>)
        : (<>
            <CardHeader className={classNames({
                'bg-danger': errorMessage,
                'bg-primary': !errorMessage
            }, 'text-white'
            )}>
                {errorMessage ? 'Something went wrong :(' : 'Create new randomized game'}
            </CardHeader>
            <CardBody>
                {errorMessage || 'Please wait, loading...'}
            </CardBody>
        </>);

    return (
        <Container>
            <Row className="justify-content-md-center">
                <Col md="10">
                    <Card>
                        {component}
                    </Card>
                </Col>
            </Row>
            <Modal isOpen={modal} backdrop="static" autoFocus>
                <ModalHeader>Generating new game</ModalHeader>
                <ModalBody>
                    <p>Please wait while a new game is generated</p>
                    <Progress animated color="info" value={100} />
                </ModalBody>
            </Modal>
        </Container>
    );

    function createFormOption({ type, key, description, values }) {
        return type === 'seed' ? (
            <InputGroup prefix={description}>
                <InputWithoutSpinner type="number" min={0} max={0x7FFF_FFFF} value={options[key]}
                    onChange={(e) => updateOption(key, e.target.value)}
                />
            </InputGroup>
        )
        : type === 'dropdown' ? (
            <InputGroup prefix={description}>
                <Input type="select" value={options[key]} onChange={(e) => updateOption(key, e.target.value)}>
                    {map(values, (v, k) => <option key={k} value={k}>{v}</option>)}
                </Input>
            </InputGroup>
        )
        : type === 'checkbox' ? (
            <InputGroup prefixClassName="mr-1" prefix={description}>
                <BootstrapSwitchButton onlabel="Yes" offlabel="No" width="80" checked={options[key] === 'true'}
                    onChange={checked => updateOption(key, checked.toString())}
                />
            </InputGroup>
        )
        : type === 'players' && options.gamemode === 'multiworld' ? (
            <InputGroup prefix={description}>
                <Input value={options[key]} onChange={(e) => updateOption(key, e.target.value)} />
            </InputGroup>
        )
        : null;
    }

    function createPlayerInputs() {
        /* Chromium did not respect "off", so we're forced to use "new-passwod" */
        return map(range(parseInt(options.players)), (p) => (
            <InputGroup prefix={`Name ${p + 1}`}>
                <Input autoComplete="new-password" value={names[p] || ''} required pattern=".*[A-Za-z\d].*"
                    onChange={(e) => { playerPatternValidity(e.target); setNames({ ...names, [p]: e.target.value }); }}
                />
            </InputGroup>
        ));
    }

    function playerPatternValidity(element) {
        /* A custom message sets the ordinary message, so it first has to be
         * reset to avoid having the suffix repeat at each validation check */
        element.setCustomValidity('');
        element.setCustomValidity(element.validity.patternMismatch
            ? `${element.validationMessage} (Must contain at least one letter or digit)`
            : ''
        );
    }

}
