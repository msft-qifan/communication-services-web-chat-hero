import { PrimaryButton, Stack } from '@fluentui/react';
import React from 'react';

import {
  copyLinkButtonStyle,
  footerMainTextStyle,
  paneFooterTokens,
} from './styles/SidePanel.styles';
import {
  inviteFooterStackContainerStyles,
  inviteFooterStackStyles,
  saveButtonTextStyle
} from './styles/SidePanel.styles';

export interface BotManagementProps {
  addBotToThread(): void;
  removeBotFromThread(): void;
}

export default (props: BotManagementProps): JSX.Element => {
  return (
    <Stack
      styles={inviteFooterStackStyles}
      className={inviteFooterStackContainerStyles}
      tokens={paneFooterTokens}
    >
      <div className={footerMainTextStyle}>Bot Management</div>
      <PrimaryButton
        tab-index="-1"
        id="addBotButton"
        className={copyLinkButtonStyle}
        onClick={() => props.addBotToThread()}
      >
        <div className={saveButtonTextStyle}>Invite</div>
      </PrimaryButton>
      <PrimaryButton
        tab-index="-1"
        id="removeBotButton"
        className={copyLinkButtonStyle}
        onClick={() => props.removeBotFromThread()}
      >
        <div className={saveButtonTextStyle}>Remove</div>
      </PrimaryButton>
    </Stack>
  );
};
