import React from 'react';
import { createGlobalStyle } from 'styled-components';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const GlobalStyle = createGlobalStyle`
  html {
    font-size: 14px;
    @media(min-width: 768px) {
      font-size: 16px;
    }
  }
`;

export default function Layout(props) {
    return (
        <>
            <GlobalStyle />
            <NavMenu gameId={props.gameId} />
            <Container className="mb-5">
                {props.children}
            </Container>
        </>
    );
}
