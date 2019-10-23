import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import { fetchRootFolders, deleteRootFolder } from 'Store/Actions/settingsActions';
import RootFolders from './RootFolders';

function createMapStateToProps() {
  return createSelector(
    (state) => state.settings.rootFolders,
    (rootFolders) => {
      return {
        ...rootFolders
      };
    }
  );
}

const mapDispatchToProps = {
  fetchRootFolders,
  deleteRootFolder
};

class RootFoldersConnector extends Component {

  //
  // Lifecycle

  componentDidMount() {
    this.props.fetchRootFolders();
  }

  //
  // Listeners

  onConfirmDeleteRootFolder = (id) => {
    this.props.deleteRootFolder({ id });
  }

  //
  // Render

  render() {
    return (
      <RootFolders
        {...this.props}
        onConfirmDeleteRootFolder={this.onConfirmDeleteRootFolder}
      />
    );
  }
}

RootFoldersConnector.propTypes = {
  fetchRootFolders: PropTypes.func.isRequired,
  deleteRootFolder: PropTypes.func.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(RootFoldersConnector);
