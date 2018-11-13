import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';

class Footer extends React.Component {
  render() {
    return (
      <footer className="footer-wrap">
        <div className="wrapper">
          <div className="footer-col footer-copyright idesc">
            <p>© SkyPrint, 2018<br />
              Полиграфические услуги.
              </p>
          </div>

          <div className="footer-col">
            <ul id="footer-nav-list" className="footer-nav-list"><li><a href="http://499363.ru/o-kompanii/">О компании</a></li>
              <li><a href="http://499363.ru/oplata-i-dostavka/">Оплата и доставка</a></li>
              <li><a href="http://499363.ru/trebovaniya-k-maketam/">Требования к макетам</a></li>
              <li><a href="http://499363.ru/kontakty/">Контакты</a></li>
            </ul>                </div>

          <div className="footer-col footer-contacts idesc">
            <p>E-mail: <a href="mailto:567@499363.ru">567@499363.ru</a></p>
            <p>г. Омск, ул. Ильинская 4, офис 61</p>
            <p><a href="#box-map-go" data-fancybox="">Карта проезда</a></p>
          </div>
        </div>
      </footer>
    );
  }
}

export default connect(
  state => state,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Footer);
