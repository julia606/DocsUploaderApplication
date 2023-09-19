import React, { Component } from 'react';
import axios from 'axios';
import './Home.css';
import { Modal, Button } from 'react-bootstrap';

export class Home extends Component {
    emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

    state = {
        selectedFile: null,
        selectedEmail: "",
        emailError: "",
        fileError: "",
        showModal: false,
        message: ''
    };

    handleEmailChange = (e) => {
        const selectedEmail = e.target.value;
        this.setState({ selectedEmail });

        if (!this.validateEmail(selectedEmail)) {
            this.setState({ emailError: "Invalid email address" });
        }
        else {
            this.setState({ emailError: "" });
        }
    };

    handleFileChange = (e) => {
        const file = e.target.files[0];
        this.setState({ selectedFile: file });

        if (!this.validateFile(file)) {
            this.setState({ fileError: "Invalid file format" });
        }
        else {
            this.setState({ fileError: "" });
        }
    };

    validateEmail = (email) => {
        return this.emailRegex.test(email);
    };

    validateFile = (file) => {
        return file && file.name.toLowerCase().endsWith(".docx");
    };

    handleUpload = () => {
        const formData = new FormData();
        formData.append('file', this.state.selectedFile);
        formData.append('email', this.state.selectedEmail);

        axios
            .post('file', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then((response) => {
                this.setState({ message: "Success!\n Wait for notification on your email, when file will be added to a storage", showModal: true });
            })
            .catch((error) => {
                console.error(error);
                this.setState({ message: 'Something went wrong, please try again', showModal: true });
            });
    };

    handleCloseModal = () => {
        this.setState({
            showModal: false,
            message: '',
            selectedFile: null, 
            selectedEmail: '', 
        });
    };

    render() {
        return (
            <div className="container">
                <div className="form-group">
                    <h2>File Uploader</h2>
                    <label>Enter your email:</label>
                    <input
                        type="email"
                        className="form-control"
                        onChange={this.handleEmailChange}
                        required
                    />
                    <div className="text-danger">{this.state.emailError}</div> { }
                    <br />
                    <label>Select a file:</label>
                    <input
                        type="file"
                        accept=".docx"
                        className="form-control form-control-sm"
                        id="formFileSm"
                        onChange={this.handleFileChange}
                        required
                    />
                    <div className="text-danger">{this.state.fileError}</div> { }
                    <br></br>
                    <div className="btn-container">
                        <button
                            type="submit"
                            className="btn btn-primary"
                            data-bs-toggle="button"
                            autoComplete="off"
                            onClick={this.handleUpload}
                        >
                            Upload
                        </button>
                    </div>
                </div>

                {/*Modal window*/}
                <Modal show={this.state.showModal} onHide={this.handleCloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>Upload Status</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>{this.state.message}</Modal.Body>
                    <Modal.Footer>
                        <Button variant="primary" onClick={this.handleCloseModal}>
                            Close
                        </Button>
                    </Modal.Footer>
                </Modal>
            </div>

        );
    }
}
