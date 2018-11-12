import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/order';
import { Banner } from './bannerComponent';
import { InfoField } from './infoFieldComponent';
import { Spinner } from './spinnerComponent';
import { parsingUrl } from '../service/helper';

class Main extends React.Component {
  componentDidMount() {
    this.props.loadDataAction(parsingUrl(this.props.routing.location.search));
  }
  render() {
    return (
      <main>
        {this.props.order.isLoading ?
          (
            <Spinner />
          )
          :
          (
            <React.Fragment>
              <Banner />
              <InfoField
                order={this.props.order}
                showAmendmentsModalAction={this.props.showAmendmentsModalAction}
              />
            </React.Fragment>
          )
        }
      </main>
    );
  }
}

export default connect(
  state => state,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Main);
