import PropTypes from 'prop-types';
import React, { Component } from 'react';
import sortByName from 'Utilities/Array/sortByName';
import { icons } from 'Helpers/Props';
import FieldSet from 'Components/FieldSet';
import Card from 'Components/Card';
import Icon from 'Components/Icon';
import PageSectionContent from 'Components/Page/PageSectionContent';
import RootFolder from './RootFolder';
import EditRootFolderModalConnector from './EditRootFolderModalConnector';
import styles from './RootFolders.css';

class RootFolders extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      isEditRootFolderModalOpen: false
    };
  }

  //
  // Listeners

  onPress = () => {
    this.setState({ isEditRootFolderModalOpen: true });
  }

  onEditRootFolderModalClose = () => {
    this.setState({ isEditRootFolderModalOpen: false });
  }

  //
  // Render

  render() {
    const {
      items,
      onConfirmDeleteRootFolder,
      ...otherProps
    } = this.props;

    const {
      isEditRootFolderModalOpen
    } = this.state;

    return (
      <FieldSet legend="Root Folders">
        <PageSectionContent
          errorMessage="Unable to load Root Folders"
          {...otherProps}
        >
          <div className={styles.rootFolders}>
            {
              items.sort(sortByName).map((item) => {
                return (
                  <RootFolder
                    key={item.id}
                    {...item}
                    onConfirmDeleteRootFolder={onConfirmDeleteRootFolder}
                  />
                );
              })
            }

            <Card
              className={styles.addRootFolder}
              onPress={this.onPress}
            >
              <div className={styles.center}>
                <Icon
                  name={icons.ADD}
                  size={45}
                />
              </div>
            </Card>
          </div>

          <EditRootFolderModalConnector
            isOpen={isEditRootFolderModalOpen}
            onModalClose={this.onEditRootFolderModalClose}
          />
        </PageSectionContent>
      </FieldSet>
    );
  }
}

RootFolders.propTypes = {
  isFetching: PropTypes.bool.isRequired,
  error: PropTypes.object,
  items: PropTypes.arrayOf(PropTypes.object).isRequired,
  onConfirmDeleteRootFolder: PropTypes.func.isRequired
};

export default RootFolders;
