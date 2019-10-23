import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import createProviderSettingsSelector from 'Store/Selectors/createProviderSettingsSelector';
import { setRootFolderValue, saveRootFolder } from 'Store/Actions/settingsActions';
import EditRootFolderModalContent from './EditRootFolderModalContent';

function createMapStateToProps() {
  return createSelector(
    (state) => state.settings.advancedSettings,
    createProviderSettingsSelector('rootFolders'),
    (advancedSettings, rootFolder) => {
      return {
        advancedSettings,
        ...rootFolder
      };
    }
  );
}

const mapDispatchToProps = {
  setRootFolderValue,
  saveRootFolder
};

class EditRootFolderModalContentConnector extends Component {

  //
  // Lifecycle

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.isSaving && !this.props.isSaving && !this.props.saveError) {
      this.props.onModalClose();
    }
  }

  //
  // Listeners

  onInputChange = ({ name, value }) => {
    this.props.setRootFolderValue({ name, value });
  }

  onSavePress = () => {
    this.props.saveRootFolder({ id: this.props.id });
  }

  //
  // Render

  render() {
    return (
      <EditRootFolderModalContent
        {...this.props}
        onSavePress={this.onSavePress}
        onInputChange={this.onInputChange}
      />
    );
  }
}

EditRootFolderModalContentConnector.propTypes = {
  id: PropTypes.number,
  isFetching: PropTypes.bool.isRequired,
  isSaving: PropTypes.bool.isRequired,
  saveError: PropTypes.object,
  item: PropTypes.object.isRequired,
  setRootFolderValue: PropTypes.func.isRequired,
  setRootFolderFieldValue: PropTypes.func.isRequired,
  saveRootFolder: PropTypes.func.isRequired,
  onModalClose: PropTypes.func.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(EditRootFolderModalContentConnector);
