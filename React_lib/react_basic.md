# React Basic
=============================
1. Data Change: (mutation or not)
```
//with mutation
var player = {score: 1, name: 'Jeff'};
player.score = 2;
// Now player is {score: 2, name: 'Jeff'}

//without mutation better because easier undo/redo and time travel
// and tracking change and determining when to re-render in React
// shouldComponentUpdate()
var player = {score: 1, name: 'Jeff'};
var newPlayer = Object.assign({}, player, {score: 2});
```
2. React.Component & Functional Components
```
//React.Component
class Square extends React.Component {
  render() {
    return (
      <button className="square" onClick={() => this.props.onClick()}>
        {this.props.value}
      </button>
    );
  }
}
//Functional Components
function Square(props) {
  return (
    <button className="square" onClick={props.onClick}>
      {props.value}
    </button>
  );
}
```