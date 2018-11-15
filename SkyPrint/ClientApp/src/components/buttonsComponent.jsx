import React from 'react';

export class ButtonsComponent extends React.Component {
  openAmendments = () => {
    this.props.showAmendmentsModalAction(true);
  };
  openApprove = () => {
    this.props.showApprovedModalAction(true);
  };
  render() {
    return (
      <div className="o-btn-container">
        <div
          name="download"
          className="download-file"
        >
          <a
            href={this.props.order.picture}
            className="o-btn-primary o-btn-info-field"
            target="_blank"
          >
            Скачать
            </a>
        </div>
        {!this.props.order.hasClientAnswer &&
          <div
            name="amendments"
            onClick={this.openAmendments}
            className="amendments"
          >
            <span className="o-btn-primary o-btn-info-field">
              Внести правки
            </span>
          </div>}
        {!this.props.order.hasClientAnswer &&
          <div
            name="approve"
            onClick={this.openApprove}
            className="approve"
          >
            <span className="o-btn-success o-btn-info-field">
              Утвердить в печать
            </span>
          </div>}
      </div>
    );
  }
}
