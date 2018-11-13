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
    if(this.props.order.isOrderNotFound) {
      return (
        <OrderNotFound
          name={this.props.routing.location.search}
        />
      )
    } else {
    return (
      <main>
        {this.props.routing.location.search ?
          this.props.order.isLoading ?
            (
              <Spinner />
            )
            :
            (
              <React.Fragment>
                <Banner />
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
            <div>
              Заказ не найден
            </div>
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
