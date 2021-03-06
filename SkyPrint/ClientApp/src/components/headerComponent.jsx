﻿import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';

class Header extends React.Component {
  render() {
    return (
      <header className="header-wrap clearfix">
        <div className="header-heads">
          <div className="wrapper header-logo-container">
            <div className="header-logo">
              <a href="http://499363.ru/">
                <img src="/Logo.png" alt="" />
              </a>
            </div>
          </div>
        </div>
      </header>
    );
  }
}

export default connect(
  state => state,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Header);
