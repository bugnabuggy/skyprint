import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';
import { Banner } from './bannerComponent';

import { Spinner } from './spinnerComponent';
import { parsingUrl } from '../service/helper';
import { OrderNotFound } from './orderNotFoundComponent';
import { OrderInfo } from './orderInfo';


class Main extends React.Component {
  closeAmendments = () => {
    this.props.showAmendmentsModalAction(false);
  };
  closeApprove = () => {
    this.props.showApprovedModalAction(false);
  };
  closeFeedback = () => {
    this.props.showFeedbackModalAction(false);
  };
  componentDidMount() {
    if (this.props.routing.location.search) {
      this.props.loadDataAction(parsingUrl(this.props.routing.location.search));
    }
  }

  render() {

    const orderData = this.props.order.isLoading && this.props.routing.location.search
      ? <Spinner />
      : <OrderInfo
        order={this.props.order}
        orderId={parsingUrl(this.props.routing.location.search)}
        sendAmendments={this.props.sendAmendmentsAction}
        showAmendmentsModalAction={this.props.showAmendmentsModalAction}
        showApprovedModalAction={this.props.showApprovedModalAction}
        showFeedbackModalAction={this.props.showFeedbackModalAction}
        showSplashScreenBannerAction={this.props.showSplashScreenBannerAction}
        closeModal={this.closeAmendments}
        closeApprove={this.closeApprove}
        closeFeedback={this.closeFeedback}
      />

    const content = this.props.order.isOrderNotFound || !this.props.routing.location.search
      ? <OrderNotFound
        name="Данный заказ не найден"
      />
      : orderData

    return (<main>
      <React.Fragment>
        <Banner />
        {content}
      </React.Fragment>
    </main>)
  }


}

export default connect(
  state => state,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Main);
