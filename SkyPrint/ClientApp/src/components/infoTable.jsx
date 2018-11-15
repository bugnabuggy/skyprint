import React from 'react';
import { Table } from 'react-bootstrap';

export class InfoTable extends React.Component {
  render() {
    return (
      <table className="table-order">
        <thead className="thead-background">
          <tr>
            <th className="text-align-center">Адрес доставки</th>
            <th className="text-align-center">Транспортная компания</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td className="text-align-center">{this.props.order.address}</td>
            <td className="text-align-center">{this.props.order.transportCompany}</td>
          </tr>
        </tbody>
      </table>
    );
  }
}
