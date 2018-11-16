import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';
import { Banner } from './bannerComponent';
import { InfoField } from './infoFieldComponent';
import { Spinner } from './spinnerComponent';
import { parsingUrl } from '../service/helper';
import { OrderNotFound } from './orderNotFoundComponent';
import { MakeEdits } from './modalMakeEditsComponent';
import { ArrowComponent } from './arrowComponent';
import { ModalApproved } from './modalApproved';

class Main extends React.Component {
  closeAmendments = () => {
    this.props.showAmendmentsModalAction(false);
  };
  closeApprove = () => {
    this.props.showApprovedModalAction(false);
  };
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
                  <div>
                    <InfoField
                      order={this.props.order}
                      orderId={parsingUrl(this.props.routing.location.search)}
                      sendAmendments={this.props.sendAmendmentsAction}
                      showAmendmentsModalAction={this.props.showAmendmentsModalAction}
                      showApprovedModalAction={this.props.showApprovedModalAction}
                    />
                    {this.props.order.isShowAmendmentsModal && <MakeEdits
                      showAmendments={this.props.order.isShowAmendmentsModal}
                      orderId={parsingUrl(this.props.routing.location.search)}
                      closeModal={this.closeAmendments}
                      sendAmendments={this.props.sendAmendmentsAction}
                    />}
                    {this.props.order.isShowApprovedModal && <ModalApproved
                      show={this.props.order.isShowApprovedModal}
                      orderId={parsingUrl(this.props.routing.location.search)}
                      sendAmendments={this.props.sendAmendmentsAction}
                      closeApprove={this.closeApprove}
                    />}
                  </div>
                  <ArrowComponent />
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
