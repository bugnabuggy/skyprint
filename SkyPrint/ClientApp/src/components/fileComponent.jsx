import React from 'react';

export class FileComponent extends React.Component {

    render() {
        const text = this.props.file ? this.props.file.name : 'Прикрепить файл';
        const isFile = this.props.file ? false : true;
        return (
        <div className="file-fiel-container">
            <img src="/clip.png" className="file-field-image" alt="" />
            <div className="file-download-link">
                <input type="file" onChange={(e) => this.props.handleFile(e, this.props.index)} alt="Выберете файл" />
                <strong>
                    <a>
                        {text}
                    </a>
                </strong>
                <img hidden={isFile} src="/CloseIcon.png" alt="remove file" className="close-icon-size" onClick={(e) => this.props.handleDeleteFile(this.props.index)}></img>
            </div>
        </div>
        );
    }

}