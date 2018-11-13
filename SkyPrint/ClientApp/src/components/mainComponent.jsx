import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';
import { Banner } from './bannerComponent';
import { InfoField } from './infoFieldComponent';
import { Spinner } from './spinnerComponent';
import { parsingUrl } from '../service/helper';
import { OrderNotFound } from './orderNotFoundComponent';

class Main extends React.Component {
  componentDidMount() {
    if (this.props.routing.location.search) {
      this.props.loadDataAction(parsingUrl(this.props.routing.location.search));
    }
  }
  render() {
    if (this.props.order.isOrderNotFound) {
      return (
        <main>
          <React.Fragment>
            <Banner />
            <OrderNotFound
              name="Данный заказ не найден"
            />
          </React.Fragment>
        </main>
      )
    } else {
      return (
        <main>
          <Banner />
          {this.props.routing.location.search ?
            this.props.order.isLoading ?
              (
                <Spinner />
              )
              :
              (
                <React.Fragment>
                  <InfoField
                    order={this.props.order}
                    orderId={parsingUrl(this.props.routing.location.search)}
                    showAmendmentsModalAction={this.props.showAmendmentsModalAction}
                    sendAmendments={this.props.sendAmendmentsAction}
                  />
                </React.Fragment>
              )
            :
            (
              <OrderNotFound
                name="Заказ не найден"
              />
            )
          }
        </main>
      );
    }
  }
}

export default connect(
  state => state,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Main);
