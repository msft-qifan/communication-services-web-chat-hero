import { connect } from 'react-redux';
import BotManagement from '../components/BotManagement';

import { State } from '../core/reducers/index';
import { addBotToThread, removeBotFromThread } from '../core/sideEffects';

const mapStateToProps = (state: State) => ({
});

const mapDispatchToProps = (dispatch: any) => ({
  addBotToThread: () => dispatch(addBotToThread()),
  removeBotFromThread: () => dispatch(removeBotFromThread())
});

export default connect(mapStateToProps, mapDispatchToProps)(BotManagement);
