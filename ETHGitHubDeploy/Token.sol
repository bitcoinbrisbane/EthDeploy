pragma solidity ^0.4.23;

import "./ERC20.sol";
import "./Ownable.sol";
import "./SafeMath.sol";

contract Token is Ownable {
    using SafeMath for uint;

    string public name = "Hot Potatoe";
    string public symbol = "HP";
    uint8 public decimals = 18;
    uint256 public totalSupply = 0;

    address private owner;

    mapping (address => uint256) balances;
    mapping (address => mapping (address => uint256)) allowed;

    uint256 public lastPricePerToken;
    uint256 public increaseValue;

    enum Growth {
        Linear, 
        Exponential 
    }

    //finney public pricePerToken;

    constructor () public {
        owner = msg.sender;
        balances[owner] = 0;
        lastPricePerToken = 0;
    }

    function balanceOf(address who) public view returns (uint256) {
        return balances[who];
    }

    function transfer(address to, uint256 value) public 
        isValidAddress(to)
        returns (bool) 
    {
        require(balances[msg.sender] >= value);

        balances[msg.sender] = balances[msg.sender].sub(value);
        balances[to] = balances[to].add(value);

        emit Transfer(msg.sender, to, value);
        return true;
    }

    function transferFrom(address from, address to, uint256 value) public 
        isValidAddress(to) 
        isValidAddress(from) 
        returns (bool)
    {
        require(balances[from] >= value && allowed[from][msg.sender] >= value && balances[to] + value >= balances[to]);

        balances[from] = balances[from].sub(value);
        allowed[from][msg.sender] = allowed[from][msg.sender].sub(value);
        balances[to] = balances[to].add(value);

        emit Transfer(from, to, value);
        return true;
    }

    function approve(address spender, uint256 amount) public returns (bool) {
        require(spender != address(0));
        require(allowed[msg.sender][spender] == 0 || amount == 0);

        allowed[msg.sender][spender] = amount;
        emit Approval(msg.sender, spender, amount);
        return true;
    }

    function buy() public payable {
        //uint amountToBuy = msg.value;

        increaseValue += 10;
    }

    function () public {
        buy();
    }

    modifier isValidAddress(address value) {
        require(value != address(0));
        _;
    }


    event Approval(address indexed owner, address indexed spender, uint256 value);
    event Transfer(address indexed from, address indexed to, uint256 value);
}