import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Counter';

class Header extends React.Component {
    render() {
        return (
            <header className="header-wrap clearfix">
                <div className="header-heads">
                    <div className="wrapper">
                        <div className="header-logo">
                            <a href="/">
                                <img src="http://499363.ru/wp-content/themes/skyprint/_img/logo.png" alt="" />
                            </a>
                        </div>
                        <div
                            className="smart-btn-header-nav"
                            id="btn-js-smart-menu"
                            data-nav-toggle="smart-header-nav-show"
                        >
                            <span className="icon-btn-header-nav"></span>
                        </div>
                        <div
                            className="header-contact-recall"
                        >
                            <span
                                className="o-btn"
                                data-obox="form-recall"
                            >
                                Перезвоните мне
                            </span>
                        </div>
                        <div className="header-contact-block type-tel">
                            <p>
                                <a href="tel:88005509163,8(3812)499363">8 800 550-91-63, 8 (3812) 499-363</a>
                            </p>
                            <p>Бесплатные звонки по РФ</p>
                            <span className="ics ic-header-tel"></span>
                        </div>
                        <div className="header-contact-block type-addr">
                            <p>Ильинская 4, офис 61</p>
                            <p>работаем с 9:00 до 18:00</p>
                            <span className="ics ic-header-addr"></span>
                        </div>
                    </div>
                </div>
                <div className="header-bottom">
                    <div className="header-bottom-foots">
                        <div className="header-contact-block type-addr">
                            <p>Ильинская 4, офис 61</p>
                            <p>работаем с 9:00 до 18:00</p>
                            <span className="ics ic-header-addr"></span>
                        </div>
                        <div className="header-contact-recall"><span className="o-btn" data-obox="form-recall">Перезвоните мне</span></div>
                    </div>
                </div>
            </header>
        );
    }
}

export default connect(
    state => state.counter,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Header);
